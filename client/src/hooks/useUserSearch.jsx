import { useState, useEffect } from 'react';
import { fetchUserByUsername, sendFriendRequest, fetchUserById } from '../api/userService';
import { useUser } from '../context/UserContext';


const useUserSearch = (onRequestSent) => {
  const [searchParam, setSearchParam] = useState('');
  const [users, setUsers] = useState([]);
  const [error, setError] = useState('');
  const [userFound, setUserFound] = useState(false);
  const { user } = useUser();

  useEffect(() => {
    const delayDebounceFn = setTimeout(async () => {
      if (!searchParam) {
        setUsers([]);
        setError('Please enter a username.');
        setUserFound(false);
        return;
      }
      try {
        const result = await fetchUserByUsername(searchParam);
        console.log('Search result:', result);
        const { user } = result; // Destructuring to get the 'user' object
        
      if (user && user.userId) {
          setUsers([user]); // Use the 'user' object for the user state
          console.log('id: ', user.userId);
          setUserFound(true);
          setError('');
        } else {
          setError('No user found with that username.');
          setUsers([]);
          setUserFound(false);
        }
      } catch (err) {
        setError(err.message);
        setUsers([]);
        setUserFound(false);
      }
    }, 500);
  

    return () => clearTimeout(delayDebounceFn);
  }, [searchParam, user?.username]); 

  const handleSendFriendRequest = async (userId) => {
    try 
    {
      await sendFriendRequest(userId);
      console.log("Friend request sent successfully!");

      const userResponse = await fetchUserById(userId);
      if (userResponse && userResponse.username) 
      {
        onRequestSent({
          id: userId,
          username: userResponse.username,
        });
      }
    } 
    catch (error) 
    {
      console.error("Failed to send friend request:", error);
    }
  };

  return {
    searchParam,
    setSearchParam,
    users,
    userFound,
    error,
    handleSendFriendRequest,
  };
};

export default useUserSearch;
