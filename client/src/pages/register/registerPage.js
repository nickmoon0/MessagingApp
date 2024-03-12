// RegisterPage.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useFormState } from '../../hooks/useFormState'; // Assuming this hook is implemented as shown
import InputField from '../../components/inputField';
import PasswordInputField from '../../components/PasswordInputField';
import FormButton from '../../components/FormButton';
import { Tooltip } from 'antd';
import { InfoCircleOutlined } from '@ant-design/icons';
import { Divider} from "@nextui-org/react";
import { register } from '../../api/userService'; // Adjust the import path as necessary
import AuthContainer from '../../components/layout/AuthContainer';



function RegisterPage() {
  const navigate = useNavigate();
  const { formData, handleChange, handleSubmit, error } = useFormState({
    username: '',
    password: '',
  }, async (data) => {
    try {
      await register(data);
      navigate('/'); // Adjust as needed for your routing
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
          <FormButton type="submit" text="Sign Up" style={{ marginTop: '27px' }} />
        </form>
        <Divider style={{ marginTop: '20px' }} />
        <FormButton text="Log in" onClick={() => navigate('/')} style={{ marginTop: '17px' }} />
        <h2 
          style={{ marginTop: '12px', textAlign: 'center', fontSize: '13px' }}>
            Have an account?
          </h2>
          </AuthContainer>
  );
}

export default RegisterPage;
