import { notification } from 'antd';
import { makeApiRequest } from './apiService';


export const register = (userData) => makeApiRequest('auth/register', 'POST', userData);



export const authenticate = (loginData) => makeApiRequest('auth/login', 'POST', loginData);

export const handleLogin = async (loginData) => {
  try {
    const response = await authenticate(loginData);
    if (response && response.accessToken) {
      localStorage.setItem('token', response.accessToken); // Make sure to store the correct token
      console.log('Token stored:', response.accessToken); // Confirm what's stored
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
    // Call the makeApiRequest function with the appropriate parameters
    const url = `user/username/${username}`;
    const method = 'GET';
    const data = null; // No body data is needed for GET requests
    const requiresAuth = true; // Assuming this endpoint requires authentication

    // Execute the request using the common API request function
    const response = await makeApiRequest(url, method, data, requiresAuth);

    // Since makeApiRequest already handles the response parsing, you can directly return the response
    return response; // This data should now include the user details as specified by your API
  } catch (error) {
    console.error('Failed to fetch user by username:', error);
    throw error;
  }
};


export const sendFriendRequest = async (toUserId) => {
  return makeApiRequest(`user/${toUserId}/add`, 'POST', null, true);
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
