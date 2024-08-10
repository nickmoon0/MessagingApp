import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useFormState } from '../../hooks/useFormState';
import InputField from '../../components/inputField';
import PasswordInputField from '../../components/PasswordInputField';
import FormButton from '../../components/FormButton';
import './loginCss.css';
import { handleLogin } from '../../api/userService';
import { Divider } from "@nextui-org/react";
import AuthContainer from '../../components/Layout/AuthContainer';
import { useUser } from '../../context/UserContext';
import AnimatedCloud from './clouds';

function LoginPage() {
  const navigate = useNavigate();
  const { setUser } = useUser();
  const [ TriggerAnimation, setTriggerAnimation] = useState(false);

  const handleSuccessfulLogin = async (data) => {
    const success = await handleLogin(data);
    if (success) {
      setUser({ username: data.username });
      setTriggerAnimation(true); 
      setTimeout(() => {
        navigate('/friends'); 
      }, 1000); 
    } else {
      throw new Error('Login failed. Please try again.');
    }
  };

  const { formData, handleChange, handleSubmit, error } = useFormState({
    username: '',
    password: '',
  }, handleSuccessfulLogin);

  const isDisabled = !formData.username || !formData.password;


  return (
    <AuthContainer>
      <div className="login-form-container">
      <AnimatedCloud src="/Cloud2.png" className="cloud2"  /> {/*middle*/}
      <AnimatedCloud src="/Cloud3.png" className="cloud3"  /> {/*bottom*/}
      <AnimatedCloud src="/cloud.png" className="cloud4"  /> {/*bottom*/}
      <AnimatedCloud src="/Cloud2.png" className="cloud5"  />  {/*middle*/}
      <AnimatedCloud src="/Cloud4.png" className="cloud6"  /> {/*top*/}
      <AnimatedCloud src="/Cloud5.png" className="cloud7"  /> {/*bottom*/}
      <AnimatedCloud src="/Cloud5.png" className="cloud8"  />  {/*middle*/}
      <AnimatedCloud src="/Cloud2.png" className="cloud10"  /> {/*top*/}
      <AnimatedCloud src="/Cloud6.png" className="cloud11"  /> {/*middle*/}
      <AnimatedCloud src="/Cloud4.png" className="cloud9"  />  {/*middle*/}
      <AnimatedCloud src="/Cloud7.png" className="cloud12"  /> {/*top*/}
      <AnimatedCloud src="/Cloud8.png" className="cloud13"  /> {/*top*/}
      <AnimatedCloud src="/Cloud9.png" className="cloud14"  /> {/*top*/}
  

      <h2 className='LoginTitleAlign'>Log in to Talk.</h2>
        <form onSubmit={handleSubmit}>
          <div className="loginSpacing">
            <InputField
              name="username"
              value={formData.username}
              onChange={handleChange}
              placeholder="Username"
              style={{ borderRadius: '10px'}}
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
          <FormButton type="submit" text="Log in" style={{ marginTop: '30px', borderRadius: '10px', border:'none' }} isDisabled={isDisabled} />

        </form>
        <Divider style={{ marginTop: '21px' }} />
        <FormButton text="Sign up" onClick={() => navigate('/register')} style={{ marginTop: '17px',  borderRadius: '10px', border:'none' }} />
      <h2 style={{ 
          marginTop: '20px', 
          textAlign: 'center', 
          fontSize: '13px' }}>
        Forgotten password?
        </h2>
        </div>

        </AuthContainer>
    );
  }
  export default LoginPage;
