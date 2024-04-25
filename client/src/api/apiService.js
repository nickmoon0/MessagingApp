
const API_BASE_URL = 'http://localhost:5134';

const getHeaders = (requiresAuth = false) => {
  const headers = {
    'Content-Type': 'application/json',
  };
  if (requiresAuth) 
  {
    const token = localStorage.getItem('token');
    headers['Authorization'] = `Bearer ${token}`;
  }
  return headers;
};


export const makeApiRequest = async (url, method, data = null, requiresAuth = false) => {
  const fullUrl = `${API_BASE_URL}/${url}`;
  const headers = getHeaders(requiresAuth);

  const options = 
  {
    method,
    headers,
  };

  if (data) 
  {
    options.body = JSON.stringify(data);
  }

  try 
  {
    const response = await fetch(fullUrl, options);

    if (!response.ok) 
    {
      throw new Error('API request failed with status ' + response.status);
    }

    const contentType = response.headers.get('content-type');
    return contentType && contentType.includes('application/json') ? response.json() : response.text();
  } 
  catch (error) 
  {
    console.error('API request error:', error.message);
    throw error;
  }
};
