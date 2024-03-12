// components/layout/AuthContainer.jsx
import React from 'react';
import {  Avatar, AvatarIcon } from "@nextui-org/react";

const AuthContainer = ({ children }) => {
  return (
    <div className="body">
        <div className="container">
    <div className="login-container">
      <div className="bubble-avatar-container">
        <div className="chat-bubble">
          {children}
        </div>
        <Avatar icon={<AvatarIcon />} style={{ marginLeft: '50px' }} size="md" color="primary" />
      </div>
    </div>
    </div>
    </div>
  );
};

export default AuthContainer;
