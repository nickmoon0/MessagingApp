// LoginPage.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useFormState } from '../../hooks/useFormState';
import InputField from '../../components/inputField';
import PasswordInputField from '../../components/PasswordInputField';
import FormButton from '../../components/FormButton';
import './loginCss.css'; 
import { handleLogin } from '../../api/userService'; 
import { Divider} from "@nextui-org/react";
import AuthContainer from '../../components/layout/AuthContainer';



function LoginPage() {
  const navigate = useNavigate();
  const { formData, handleChange, handleSubmit, error } = useFormState({
    username: '',
    password: '',
  }, async (data) => {
    const success = await handleLogin(data);
    if (success) {
      navigate('/friends'); // adjust the route as necessary
    } else {
      throw new Error('Login failed. Please try again.'); // Adjust based on your error handling
    }
  });
  const isDisabled = !formData.username || !formData.password;


  return (
    <AuthContainer>
    <h2 className='LoginTitleAlign'>Log in to Talk.</h2>
          <form onSubmit={handleSubmit}>
          <div className="loginSpacing">
            <InputField
              name="username"
              value={formData.username}
              onChange={handleChange}
              placeholder="Username"

            />
            </div>
           
            <PasswordInputField
              name="password"
              value={formData.password}
              onChange={handleChange}
              placeholder="Password"
            />
            
            <div className={`error-message-container ${error ? 'visible' : 'hidden'}`}>
          {error && <p>{error}</p>}
        </div>
            <FormButton type="submit" text="Log in" style={{ marginTop: '27px' }} isDisabled={isDisabled} />

          </form>
          <Divider style={{ marginTop: '20px' }} />
          <FormButton text="Sign up" onClick={() => navigate('/register')} style={{ marginTop: '17px' }} />
      <h2 style={{ 
            marginTop: '12px', 
            textAlign: 'center', 
            fontSize: '13px' }}>
          Forgotten password?
          </h2>
          </AuthContainer>
  );
  }

  export default LoginPage;
