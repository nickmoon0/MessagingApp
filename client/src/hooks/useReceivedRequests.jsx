import { useState, useEffect } from 'react';
import { fetchUserById } from '../api/userService';
const API_BASE_URL = 'https://localhost:7274';

const useReceivedRequests = () => {
  const [receivedRequests, setReceivedRequests] = useState([]);
  const [sentRequests, setSentRequests] = useState([]);

  useEffect(() => 
  {
    const fetchReceivedFriendRequests = async () => 
    {
      try 
      {
        const response = await fetch(`${API_BASE_URL}/friendrequest`, 
        {
          headers: 
          {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
          },
        });
        if (!response.ok) 
        {
          throw new Error('Failed to fetch received friend requests');
        }
        const data = await response.json();
        console.log("Raw friend requests data:", data);
  
        // Assuming 'id' field exists and data structure is correct
        const enrichedFriendRequests = await Promise.all(data.friendRequests.map(async (request) => 
        {
          const user = await fetchUserById(request.fromUserId);
          return {
            ...request,
            username: user.username, // Enrich with username from fetched user
            id: request.id // Ensure this line is correct based on your API
          };
        }));
  
        console.log("Enriched friend requests:", enrichedFriendRequests);
        setReceivedRequests(enrichedFriendRequests);
      } 
      catch (error) 
      {
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

return { receivedRequests, handleRequestSent };

};

export default useReceivedRequests;
