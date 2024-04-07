import { useState, useEffect } from 'react';
import { fetchFriends, acceptFriendRequest } from '../api/userService';

export const useFriends = () => {
  const [friends, setFriends] = useState([]);
  const [receivedRequests, setReceivedRequests] = useState([]);
  const [selectedTab, setSelectedTab] = useState('Friends');
  const [searchText, setSearchText] = useState('');

  useEffect(() => {
    if (selectedTab === 'Friends') {
      refreshFriendsList();
    }
  }, [selectedTab]);

  const refreshFriendsList = async () => {
    try {
      const response = await fetchFriends();
      setFriends(response.friends);
    } catch (error) {
      console.error('Failed to refresh friends list:', error);
    }
  };

  const handleAcceptFriendRequest = async (friendRequestId) => {
    try {
      await acceptFriendRequest(friendRequestId);
      const updatedRequests = receivedRequests.filter(request => request.friendRequestId !== friendRequestId);
      setReceivedRequests(updatedRequests);
      await refreshFriendsList();
    } catch (error) {
      console.error('Error accepting friend request:', error);
    }
  };

  return {
    friends, selectedTab, setSelectedTab, searchText, setSearchText,
    handleAcceptFriendRequest, refreshFriendsList
  };
};
