// components/PasswordInputField.jsx
import React from 'react';
import { Input } from 'antd';
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';

const PasswordInputField = ({ name, value, onChange, placeholder }) => (
  <Input.Password
    size="large"
    placeholder={placeholder}
    name={name}
    value={value}
    onChange={onChange}
    iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}

  />
);

export default PasswordInputField;
