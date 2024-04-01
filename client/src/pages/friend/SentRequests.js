// src/pages/FriendPage/SentRequests.jsx
import React from 'react';
import UserSearch from './UserSearch';
import SentRequestsTable from '../../components/friendsTables/SentRequestsTable';
import useSentRequests from '../../hooks/useSentRequest';

const SentRequests = () => {
  const { sentRequests, handleRequestSent, handleCancelRequest } = useSentRequests();

  const dataWithKey = sentRequests.map((request) => ({
    key: request.id,
    username: request.username,
    action: '' 
  }));

  return (
    <>
      <UserSearch onRequestSent={handleRequestSent} />
      <SentRequestsTable data={dataWithKey} onCancelRequest={handleCancelRequest} />
    </>
  );
};

export default SentRequests;
