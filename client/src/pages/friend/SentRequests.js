// src/pages/FriendPage/SentRequests.jsx
import React from 'react';
import UserSearch from './UserSearch';
import SentRequestsTable from '../../components/SentRequestsTable';
import useSentRequests from '../../hooks/useSentRequest';

const SentRequests = () => {
  const { sentRequests, handleRequestSent } = useSentRequests();

  // Prepare data for the table
  const dataWithKey = sentRequests.map((request) => ({
    key: request.id,
    username: request.username,
    action: '' // Placeholder for action buttons if needed
  }));

  return (
    <>
      <UserSearch onRequestSent={handleRequestSent} />
      <SentRequestsTable data={dataWithKey} />
    </>
  );
};

export default SentRequests;
