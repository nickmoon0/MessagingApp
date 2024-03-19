import { useState, useEffect } from 'react';
import { fetchUser, sendFriendRequest, fetchUserById } from '../api/userService';
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
        const result = await fetchUser(null, searchParam);
        // Add a condition to check if the searched user is the current user
        if (result && result.username === user?.username) {
          // Set a custom error message if the user is searching for themselves
          setError("That's you! You can't add yourself.");
          setUsers([]);
          setUserFound(false);
        } else if (result && result.id) {
          setUsers([result]);
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

  const handleSendFriendRequest = async (toUserId) => {
    try 
    {
      await sendFriendRequest(toUserId);
      console.log("Friend request sent successfully!");

      const userResponse = await fetchUserById(toUserId);
      if (userResponse && userResponse.username) 
      {
        onRequestSent({
          id: toUserId,
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
