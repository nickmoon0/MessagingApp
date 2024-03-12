// hooks/useUserSearch.js
import { useState, useEffect } from 'react';
import { fetchUser, sendFriendRequest, fetchUserById } from '../api/userService';

const useUserSearch = (onRequestSent) => {
  const [searchParam, setSearchParam] = useState('');
  const [users, setUsers] = useState([]);
  const [error, setError] = useState('');
  const [userFound, setUserFound] = useState(false);

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
        if (result && result.id) {
          setUsers([result]);
          setUserFound(true);
          setError('');
        } else {
          setUsers([]);
          setError('No user found with that username.');
          setUserFound(false);
        }
      } catch (err) {
        setError(err.message);
        setUsers([]);
        setUserFound(false);
      }
    }, 500);

    return () => clearTimeout(delayDebounceFn);
  }, [searchParam]);

  const handleSendFriendRequest = async (toUserId) => {
    try {
      await sendFriendRequest(toUserId);
      console.log("Friend request sent successfully!");

      const userResponse = await fetchUserById(toUserId);
      if (userResponse && userResponse.username) {
        onRequestSent({
          id: toUserId,
          username: userResponse.username,
        });
      }
    } catch (error) {
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
