import React from 'react';
import { Input } from 'antd';

const InputField = ({ name, value, onChange, placeholder }) => (
  <Input
    size="large"
    allowClear
    placeholder={placeholder}
    name={name}
    value={value}
    onChange={onChange}
  />
);

export default InputField;
