import { useState, useEffect } from 'react';
import { fetchSentFriendRequests, fetchUserById } from '../api/userService';
import { notification } from 'antd';

const useSentRequests = () => {
  const [sentRequests, setSentRequests] = useState([]);

  useEffect(() => {
    const getSentFriendRequests = async () => {
      try 
      {
        const response = await fetchSentFriendRequests();
        if (response.friendRequests && Array.isArray(response.friendRequests)) 
        {
          const requestsWithUsernames = await Promise.all(
            response.friendRequests.map(async (request) => {
              const userResponse = await fetchUserById(request.toUserId);
              return { ...request, username: userResponse.username };
            })
          );
          setSentRequests(requestsWithUsernames);
        } 
        else 
        {
          console.error('Received data is not in expected format:', response);
        }
      } 
      catch (error) 
      {
        console.error('Failed to fetch sent friend requests:', error);
      }
    };

    getSentFriendRequests();
  }, []);

  const handleRequestSent = (newRequest) => {
    setSentRequests((prevRequests) => [...prevRequests, newRequest]);
  };

  const handleCancelRequest = (requestId) => {
    // Remove the request from the state, simulating a successful cancellation
    setSentRequests(currentRequests => currentRequests.filter(request => request.id !== requestId));
    // Optionally show a notification
    notification.success({
      message: 'Request Cancelled',
      description: 'The friend request has been successfully cancelled.',
    });
  };

  return { sentRequests, handleRequestSent, handleCancelRequest };
};


export default useSentRequests;
