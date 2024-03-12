// src/hooks/useSentRequests.js
import { useState, useEffect } from 'react';
import { fetchSentFriendRequests, fetchUserById } from '../api/userService';

const useSentRequests = () => {
  const [sentRequests, setSentRequests] = useState([]);

  useEffect(() => {
    const getSentFriendRequests = async () => {
      try {
        const response = await fetchSentFriendRequests();
        if (response.friendRequests && Array.isArray(response.friendRequests)) {
          const requestsWithUsernames = await Promise.all(
            response.friendRequests.map(async (request) => {
              const userResponse = await fetchUserById(request.toUserId);
              return { ...request, username: userResponse.username };
            })
          );
          setSentRequests(requestsWithUsernames);
        } else {
          console.error('Received data is not in the expected format:', response);
        }
      } catch (error) {
        console.error('Failed to fetch sent friend requests:', error);
      }
    };

    getSentFriendRequests();
  }, []);

  const handleRequestSent = (newRequest) => {
    setSentRequests((prevRequests) => [...prevRequests, newRequest]);
  };

  return { sentRequests, handleRequestSent };
};

export default useSentRequests;
