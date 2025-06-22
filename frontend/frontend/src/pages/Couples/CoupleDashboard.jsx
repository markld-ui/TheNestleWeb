import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Box, Heading, Text, Button, Spinner, 
  VStack, HStack, Badge, useToast, Modal,
  ModalOverlay, ModalContent, ModalHeader, 
  ModalBody, ModalFooter, ModalCloseButton,
  Input, Code, FormControl, FormLabel
} from '@chakra-ui/react';
import { getMyCouple, createCouple, deleteCouple } from '../../api/couples';
import { generateInvite, acceptInvite } from '../../api/invites';

const CoupleDashboard = () => {
  const [couple, setCouple] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isCreating, setIsCreating] = useState(false);
  const [isGeneratingInvite, setIsGeneratingInvite] = useState(false);
  const [inviteCode, setInviteCode] = useState('');
  const [showInviteModal, setShowInviteModal] = useState(false);
  const [showAcceptModal, setShowAcceptModal] = useState(false);
  const [codeInput, setCodeInput] = useState('');
  const navigate = useNavigate();
  const toast = useToast();

  useEffect(() => {
    const fetchCouple = async () => {
      try {
        const data = await getMyCouple();
        setCouple(data);
      } catch (error) {
        console.error('Error fetching couple:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchCouple();
  }, []);

  const handleCreateCouple = async () => {
    setIsCreating(true);
    try {
      const newCouple = await createCouple();
      setCouple(newCouple);
      toast({
        title: 'Couple created',
        status: 'success',
        duration: 3000,
      });
    } catch (error) {
      toast({
        title: 'Error',
        description: error.response?.data?.message || 'Failed to create couple',
        status: 'error',
        duration: 5000,
      });
    } finally {
      setIsCreating(false);
    }
  };

  const handleGenerateInvite = async () => {
    setIsGeneratingInvite(true);
    try {
      const invite = await generateInvite(couple.coupleId);
      setInviteCode(invite.code);
      setShowInviteModal(true);
    } catch (error) {
      toast({
        title: 'Error',
        description: error.response?.data?.message || 'Failed to generate invite',
        status: 'error',
        duration: 5000,
      });
    } finally {
      setIsGeneratingInvite(false);
    }
  };

  const handleAcceptInvite = async () => {
    try {
      await acceptInvite(codeInput);
      toast({
        title: 'Invite accepted',
        description: 'You have successfully joined the couple!',
        status: 'success',
        duration: 3000,
      });
      setShowAcceptModal(false);
      // Refresh couple data
      const data = await getMyCouple();
      setCouple(data);
    } catch (error) {
      toast({
        title: 'Error',
        description: error.response?.data?.message || 'Failed to accept invite',
        status: 'error',
        duration: 5000,
      });
    }
  };

  const handleDeleteCouple = async () => {
    try {
      await deleteCouple(couple.coupleId);
      setCouple(null);
      toast({
        title: 'Couple deleted',
        status: 'success',
        duration: 3000,
      });
    } catch (error) {
      toast({
        title: 'Error',
        description: error.response?.data?.message || 'Failed to delete couple',
        status: 'error',
        duration: 5000,
      });
    }
  };

  if (loading) {
    return <Spinner size="xl" />;
  }

  return (
    <Box p={4}>
      <Heading mb={6}>My Couple</Heading>
      
      {!couple ? (
        <VStack spacing={4} align="stretch">
          <Text>You don't have a couple yet.</Text>
          <Button 
            colorScheme="blue" 
            onClick={handleCreateCouple}
            isLoading={isCreating}
          >
            Create Couple
          </Button>
          <Button 
            variant="outline" 
            onClick={() => setShowAcceptModal(true)}
          >
            Join Existing Couple
          </Button>
        </VStack>
      ) : (
        <VStack spacing={4} align="stretch">
          <Box p={4} borderWidth="1px" borderRadius="lg">
            <Text fontSize="xl" fontWeight="bold">Couple #{couple.coupleId}</Text>
            <Text>Created: {new Date(couple.createdAt).toLocaleDateString()}</Text>
            
            <HStack mt={4} spacing={4}>
              <Box flex={1} p={3} borderWidth="1px" borderRadius="md">
                <Text fontWeight="bold">User 1</Text>
                <Text>{couple.user1?.firstName} {couple.user1?.lastName}</Text>
                <Text>{couple.user1?.email}</Text>
              </Box>
              
              <Box flex={1} p={3} borderWidth="1px" borderRadius="md">
                <Text fontWeight="bold">User 2</Text>
                {couple.user2Id ? (
                  <>
                    <Text>{couple.user2?.firstName} {couple.user2?.lastName}</Text>
                    <Text>{couple.user2?.email}</Text>
                  </>
                ) : (
                  <>
                    <Text>No second user yet</Text>
                    <Button 
                      mt={2} 
                      size="sm" 
                      colorScheme="teal"
                      onClick={handleGenerateInvite}
                      isLoading={isGeneratingInvite}
                    >
                      Invite Partner
                    </Button>
                  </>
                )}
              </Box>
            </HStack>
            
            <Button 
              mt={4} 
              colorScheme="red" 
              variant="outline"
              onClick={handleDeleteCouple}
            >
              Delete Couple
            </Button>
          </Box>
        </VStack>
      )}
      
      {/* Invite Generation Modal */}
      <Modal isOpen={showInviteModal} onClose={() => setShowInviteModal(false)}>
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>Invite Code</ModalHeader>
          <ModalCloseButton />
          <ModalBody>
            <Text mb={4}>Share this code with your partner:</Text>
            <Code p={2} fontSize="xl">{inviteCode}</Code>
            <Text mt={4} fontSize="sm" color="gray.500">
              This code will expire in 7 days
            </Text>
          </ModalBody>
          <ModalFooter>
            <Button colorScheme="blue" onClick={() => setShowInviteModal(false)}>
              Close
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
      
      {/* Accept Invite Modal */}
      <Modal isOpen={showAcceptModal} onClose={() => setShowAcceptModal(false)}>
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>Join a Couple</ModalHeader>
          <ModalCloseButton />
          <ModalBody>
            <FormControl>
              <FormLabel>Enter invite code:</FormLabel>
              <Input 
                value={codeInput}
                onChange={(e) => setCodeInput(e.target.value)}
                placeholder="Invite code"
              />
            </FormControl>
          </ModalBody>
          <ModalFooter>
            <Button variant="outline" mr={3} onClick={() => setShowAcceptModal(false)}>
              Cancel
            </Button>
            <Button colorScheme="blue" onClick={handleAcceptInvite}>
              Join Couple
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </Box>
  );
};

export default CoupleDashboard;