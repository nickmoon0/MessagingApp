// userService.js

import { makeApiRequest } from './apiService';

// Function to register a new user
export const register = (userData) => makeApiRequest('Auth/Register', 'POST', userData);

// Function to authenticate (login) a user
export const authenticate = (loginData) => makeApiRequest('Auth/Authenticate', 'POST', loginData);

export const handleLogin = async (loginData) => {
  try {
    const token = await authenticate(loginData);
    localStorage.setItem('token', token);
    console.log('Token stored:', localStorage.getItem('token'));
    return true;
  } catch (error) {
    console.error('Login error:', error);
    return false;
  }
};

export const fetchUser = async (userId, username) => {
  // Construct URLSearchParams to conditionally include parameters
  const params = new URLSearchParams();
  if (userId) params.append('uid', userId);
  if (username) params.append('username', username);

  const response = await makeApiRequest(`user?${params.toString()}`, 'GET', null, true);
  return response;
};


export const sendFriendRequest = async (toUserId) => {
  return makeApiRequest(`user/${toUserId}/add`, 'POST', null, true);
};

export const fetchSentFriendRequests = () => makeApiRequest('friendrequest', 'GET', null, true);

export const fetchUserById = (userId) => makeApiRequest(`user?uid=${userId}`, 'GET', null, true);


export const acceptFriendRequest = async (friendRequestId) => {
    try {

        const response = await makeApiRequest(`FriendRequest/accept/${friendRequestId}`, 'PUT', null, true);
        console.log('Friend request accepted successfully:', response);
        return response; 
    } catch (error) {
        console.error('Error accepting friend request:', error);
        throw error; 
    }
};
