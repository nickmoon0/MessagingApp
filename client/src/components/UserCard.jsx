// components/UserCard.jsx
import React from 'react';
import { Card, Button } from 'antd';

const UserCard = ({ users, onSendRequest }) => (
  <Card size="small" style={{ marginTop: 16, border: 'none' }}>
    {users.map((user) => (
      <div
        key={user.id}
        style={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          padding: '0px 8px',
        }}
      >
        <span style={{ fontSize: '16px', lineHeight: '0' }}>{user.username}</span>
        <Button
          type="primary"
          onClick={() => onSendRequest(user.id)}
          style={{
            backgroundColor: 'green',
            borderColor: 'green',
            color: '#fff',
            left: 20,
          }}
        >
          Send Request
        </Button>
      </div>
    ))}
  </Card>
);

export default UserCard;
