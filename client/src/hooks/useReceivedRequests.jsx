import { useState, useEffect } from 'react';
import { fetchUserById } from '../api/userService';
const API_BASE_URL = 'https://localhost:7274';

const useReceivedRequests = () => {
  const [receivedRequests, setReceivedRequests] = useState([]);
    const [sentRequests, setSentRequests] = useState([]);

    useEffect(() => {
      const fetchReceivedFriendRequests = async () => {
        try {
          const response = await fetch(`${API_BASE_URL}/friendrequest`, {
            headers: {
              'Authorization': `Bearer ${localStorage.getItem('token')}`,
            },
          });
          if (!response.ok) {
            throw new Error('Failed to fetch received friend requests');
          }
          const data = await response.json();
          console.log("Raw friend requests data:", data);
  
          const enrichedFriendRequests = await Promise.all(data.friendRequests.map(async (request) => {
            const user = await fetchUserById(request.fromUserId);
            return {
              ...request,
              username: user.username,
              friendRequestId: request.friendRequestId
            };
          }));
  
          console.log("Enriched friend requests:", enrichedFriendRequests);
  
          // Deduplicate enrichedFriendRequests by username
          const uniqueUsernames = new Set();
          const deduplicatedRequests = enrichedFriendRequests.filter(request => {
            if (uniqueUsernames.has(request.username)) {
              return false;
            } else {
              uniqueUsernames.add(request.username);
              return true;
            }
          });
  
          setReceivedRequests(deduplicatedRequests);
        } catch (error) {
          console.error('Error:', error);
        }
      };
  
      fetchReceivedFriendRequests();
    }, []);
  
  
  
  
  // Inside Friends component
const handleRequestSent = (newRequest) => 
{
  // Assuming there's a state in Friends that tracks sent requests
  // For example: const [sentRequests, setSentRequests] = useState([]);
  setSentRequests((prevRequests) => [...prevRequests, newRequest]);
};

return { receivedRequests, handleRequestSent, setReceivedRequests };

};

export default useReceivedRequests;
