import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useFormState } from '../../hooks/useFormState'; 
import InputField from '../../components/inputField';
import PasswordInputField from '../../components/PasswordInputField';
import FormButton from '../../components/FormButton';
import { Tooltip } from 'antd';
import { InfoCircleOutlined } from '@ant-design/icons';
import { Divider} from "@nextui-org/react";
import { register } from '../../api/userService'; 
import AuthContainer from '../../components/Layout/AuthContainer';
import AnimatedCloud from './clouds';
import './Register.css';


function RegisterPage() {
  const navigate = useNavigate();
  const { formData, handleChange, handleSubmit, error } = useFormState({
    username: '',
    password: '',
  }, async (data) => {
    try {
      await register(data);
      navigate('/'); 
    } catch (error) {

    }
  });

  const tooltipContent = (
    <Tooltip title={
      <React.Fragment>
        Password must include at least one digit, one lowercase letter, one non-alphanumeric character,
        be at least 8 characters long, have at least 2 unique characters, and include at least one uppercase letter.
      </React.Fragment>
    } trigger="click">
      <InfoCircleOutlined style={{ color: 'rgba(0,0,0,.45)' }} />
    </Tooltip>
  );

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
  
      <form onSubmit={handleSubmit}>
        <h2 className='LoginTitleAlign'>Sign up to Talk.</h2>
          <div className="loginSpacing">
          <InputField
            name="username"
            value={formData.username}
            onChange={handleChange}
            placeholder="Username"
            suffix={tooltipContent}
          />
          </div>
          <PasswordInputField
            name="password"
            value={formData.password}
            onChange={handleChange}
            placeholder="Password"
          />
          {error && <p className="error-message">{error}</p>}
          <FormButton type="submit" text="Sign Up" style={{ marginTop: '30px', borderRadius: '10px', border:'none' }} />
      </form>
        <Divider style={{ marginTop: '21px' }} />
        <FormButton text="Log in" onClick={() => navigate('/')} style={{ marginTop: '17px',  borderRadius: '10px', border:'none' }} />
        <h2 
          style={{ marginTop: '20px', textAlign: 'center', fontSize: '13px' }}>
            Have an account?
        </h2>
        </div>
    </AuthContainer>
  );
}

export default RegisterPage;
