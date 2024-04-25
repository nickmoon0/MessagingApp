import { notification } from 'antd';
import { makeApiRequest } from './apiService';


export const register = (userData) => makeApiRequest('auth/register', 'POST', userData);



export const authenticate = (loginData) => makeApiRequest('auth/login', 'POST', loginData);

export const handleLogin = async (loginData) => {
  try {
    const response = await authenticate(loginData);
    if (response && response.accessToken) {
      localStorage.setItem('token', response.accessToken);
      console.log('Token stored:', response.accessToken); 
      return true;
    } else {
      throw new Error('Authentication token was not provided.');
    }
  } catch (error) {
    console.error('Login error:', error);
    return false;
  }
};


export const fetchUserByUsername = async (username) => {
  try {
    const url = `user/username/${username}`;
    const method = 'GET';
    const data = null; 
    const requiresAuth = true; 


    const response = await makeApiRequest(url, method, data, requiresAuth);
    return response; 
  } catch (error) {
    console.error('Failed to fetch user by username:', error);
    throw error;
  }
};


export const sendFriendRequest = async (receivingUserId) => {
  return makeApiRequest(`user/${receivingUserId}/add`, 'POST', null, true);
};

export const fetchSentFriendRequests = () => makeApiRequest('friendRequest/', 'GET', null, true);

export const fetchUserById = (userId) => makeApiRequest(`user/${userId}`, 'GET', null, true);


export const acceptFriendRequest = async (friendRequestId) => {
    try 
    {
        const response = await makeApiRequest(`FriendRequest/accept/${friendRequestId}`, 'PUT', null, true);
        notification.success({
          message: 'Friend Request Accepted',
          description: 'The friend request has been successfully accepted.',
          duration: 2.5,
      });
        return response; 
    } 
    catch (error) 
    {
        console.error('Error accepting friend request:', error);
        throw error; 
    }
};

export const fetchFriends = () => makeApiRequest('User/friends', 'GET', null, true);
