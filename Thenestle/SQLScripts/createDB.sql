-- Создание схемы
CREATE SCHEMA couple_app;

SET search_path TO couple_app;

-- Типы настроения
CREATE TABLE mood (
    mood_id SERIAL PRIMARY KEY,
    emoji VARCHAR(10) NOT NULL CHECK (emoji ~ '^[😀-🙏]$'),
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Пользователи
CREATE TABLE "user" (
    user_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE 
        CHECK (email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$'),
    password_hash TEXT NOT NULL,
    currency_balance INTEGER NOT NULL DEFAULT 0 CHECK (currency_balance >= 0),
    gender VARCHAR(10) CHECK (gender IN ('Male', 'Female', 'Other', 'Prefer not to say')),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    roles TEXT[] DEFAULT '{}'
);

-- Роли
CREATE TABLE role (
    role_id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Связь пользователей и ролей (many-to-many)
CREATE TABLE user_role (
    user_id INTEGER NOT NULL REFERENCES "user"(user_id) ON DELETE CASCADE,
    role_id INTEGER NOT NULL REFERENCES role(role_id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, role_id)
);

-- Пары
CREATE TABLE couple (
    couple_id SERIAL PRIMARY KEY,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    user1_id INTEGER NOT NULL REFERENCES "user"(user_id),
    user2_id INTEGER NOT NULL REFERENCES "user"(user_id),
    CHECK (user1_id < user2_id),
    UNIQUE (user1_id, user2_id)
);

-- Инвайты
CREATE TABLE invite (
    invite_id SERIAL PRIMARY KEY,
    code VARCHAR(8) NOT NULL UNIQUE CHECK (LENGTH(code) = 8),
    is_used BOOLEAN NOT NULL DEFAULT FALSE,
    couple_id INTEGER NOT NULL REFERENCES couple(couple_id),
    inviter_id INTEGER NOT NULL REFERENCES "user"(user_id),
    status VARCHAR(20) NOT NULL 
        CHECK (status IN ('pending', 'accepted', 'rejected', 'expired')),
    expires_at TIMESTAMPTZ NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CHECK (expires_at > created_at)
);

-- Записи настроения
CREATE TABLE mood_entry (
    mood_entry_id SERIAL PRIMARY KEY,
    mood_id INTEGER NOT NULL REFERENCES mood(mood_id),
    comment TEXT CHECK (LENGTH(comment) <= 500),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    user_id INTEGER NOT NULL REFERENCES "user"(user_id),
    CHECK (created_at <= NOW())
);

-- Напоминания
CREATE TABLE reminder (
    reminder_id SERIAL PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    remind_at TIMESTAMPTZ NOT NULL,
    type VARCHAR(20) NOT NULL CHECK (type IN ('personal', 'shared')),
    status VARCHAR(20) NOT NULL DEFAULT 'active' 
        CHECK (status IN ('active', 'completed', 'canceled')),
    created_by_id INTEGER NOT NULL REFERENCES "user"(user_id),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CHECK (remind_at > created_at)
);

-- Уведомления о напоминаниях
CREATE TABLE reminder_notification (
    notification_id SERIAL PRIMARY KEY,
    days_before INTEGER NOT NULL CHECK (days_before IN (1, 3, 7)),
    is_sent BOOLEAN NOT NULL DEFAULT FALSE,
    sent_at TIMESTAMPTZ,
    reminder_id INTEGER NOT NULL REFERENCES reminder(reminder_id),
    user_id INTEGER NOT NULL REFERENCES "user"(user_id)
);

-- Заказы еды
CREATE TABLE food_order (
    order_id SERIAL PRIMARY KEY,
    description TEXT NOT NULL,
    price INTEGER NOT NULL CHECK (price > 0),
    status VARCHAR(20) NOT NULL 
        CHECK (status IN ('pending', 'confirmed', 'canceled', 'delivered')),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    approved_at TIMESTAMPTZ,
    initiator_id INTEGER NOT NULL REFERENCES "user"(user_id),
    CHECK (approved_at >= created_at OR approved_at IS NULL)
);

-- Транзакции валюты
CREATE TABLE currency_transaction (
    transaction_id SERIAL PRIMARY KEY,
    amount INTEGER NOT NULL CHECK (amount != 0),
    type VARCHAR(20) NOT NULL 
        CHECK (type IN ('order', 'bonus', 'correction', 'refund')),
    user_id INTEGER NOT NULL REFERENCES "user"(user_id),
    order_id INTEGER REFERENCES food_order(order_id),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Места
CREATE TABLE place (
    place_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    latitude DECIMAL(9,6) NOT NULL CHECK (latitude BETWEEN -90 AND 90),
    longitude DECIMAL(9,6) NOT NULL CHECK (longitude BETWEEN -180 AND 180),
    address TEXT,
    category VARCHAR(50),
    phone VARCHAR(20) CHECK (phone ~ '^[0-9\s\-+()]+$'),
    email VARCHAR(100) CHECK (email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$'),
    added_by_id INTEGER NOT NULL REFERENCES "user"(user_id),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Устройства
CREATE TABLE device (
    device_id SERIAL PRIMARY KEY,
    device_type VARCHAR(50) NOT NULL CHECK (device_type IN ('iOS', 'Android', 'Web', 'Telegram')),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    last_active_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    user_id INTEGER NOT NULL REFERENCES "user"(user_id),
    CHECK (last_active_at <= NOW())
);

-- Добавление базовых типов настроения
INSERT INTO mood (emoji, name) VALUES 
('😊', 'Happy'),
('😢', 'Sad'),
('😡', 'Angry'),
('😍', 'In Love'),
('😴', 'Tired');

-- Добавление базовых ролей
INSERT INTO role (name) VALUES 
('User'),
('PremiumUser'),
('Admin');

-- Триггер для обновления updated_at
CREATE OR REPLACE FUNCTION update_updated_at()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Применение триггера к таблице пользователей
CREATE TRIGGER update_user_updated_at
BEFORE UPDATE ON "user"
FOR EACH ROW
EXECUTE FUNCTION update_updated_at();

-- Применение триггера к таблице напоминаний
CREATE TRIGGER update_reminder_updated_at
BEFORE UPDATE ON reminder
FOR EACH ROW
EXECUTE FUNCTION update_updated_at();

-- Триггер для обновления ролей пользователя
CREATE OR REPLACE FUNCTION update_user_roles_and_timestamp()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE "user" u
    SET 
        roles = ARRAY(
            SELECT r.name 
            FROM role r
            JOIN user_role ur ON r.role_id = ur.role_id
            WHERE ur.user_id = NEW.user_id
        ),
        updated_at = NOW()
    WHERE u.user_id = NEW.user_id;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_user_roles_insert
AFTER INSERT ON user_role
FOR EACH ROW
EXECUTE FUNCTION update_user_roles_and_timestamp();

CREATE TRIGGER update_user_roles_delete
AFTER DELETE ON user_role
FOR EACH ROW
EXECUTE FUNCTION update_user_roles_and_timestamp();

CREATE TRIGGER update_user_roles_update
AFTER UPDATE ON user_role
FOR EACH ROW
EXECUTE FUNCTION update_user_roles_and_timestamp();

-- Индексы для ускорения запросов
CREATE INDEX idx_user_email ON "user"(email);
CREATE INDEX idx_mood_entry_user_id ON mood_entry(user_id);
CREATE INDEX idx_mood_entry_created_at ON mood_entry(created_at);
CREATE INDEX idx_reminder_user_id ON reminder(created_by_id);
CREATE INDEX idx_reminder_remind_at ON reminder(remind_at);
CREATE INDEX idx_food_order_initiator_id ON food_order(initiator_id);
CREATE INDEX idx_food_order_status ON food_order(status);
CREATE INDEX idx_currency_transaction_user_id ON currency_transaction(user_id);
CREATE INDEX idx_user_role_user_id ON user_role(user_id);
CREATE INDEX idx_user_role_role_id ON user_role(role_id);