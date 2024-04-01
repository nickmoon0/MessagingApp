import React from 'react';
import { Modal, Button } from 'antd';

const SearchModal = ({ isModalVisible, handleOk, handleCancel, children }) => (
  <Modal
    title="Add User"
    visible={isModalVisible}
    onOk={handleOk}
    onCancel={handleCancel}
    footer={[
      <Button key="back" onClick={handleCancel}>
        Close
      </Button>,
    ]}
  >
    {children}
  </Modal>
);

export default SearchModal;
