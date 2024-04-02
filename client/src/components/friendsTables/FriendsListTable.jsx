import React, { useRef, useState } from 'react';
import { Table, Button, Input, Space, Tooltip, Card } from 'antd';
import { SearchOutlined, DeleteOutlined, MessageOutlined} from '@ant-design/icons';
import Highlighter from 'react-highlight-words';
import { MagnifyingGlass } from 'phosphor-react';

const FriendsListTable = ({ friends }) => {
  

  const columns = [
    {
      title: `Friends - ${friends.length}`,
      dataIndex: 'username',
      key: 'username',
      render: (text) => (
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', width: '100%' }}>
          <span style={{ fontSize: "15px", fontWeight: "600" }}>
            {text}
          </span>
          <div>
            <Tooltip placement="top" title="Delete Friend">
              <Button
                type="danger"
                shape="circle"
                icon={<DeleteOutlined />}
                className="reject-button"
                style={{ marginRight: "7px" }}
              />
            </Tooltip>
            <Tooltip placement="top" title="Message User">
              <Button
                type="default"
                shape="circle"
                icon={<MessageOutlined />}
                className="reject-button"
                style={{ marginRight: "7px" }}
              />
            </Tooltip>
          </div>
        </div>
      ),
      onHeaderCell: () => ({ style: { bottom: "8px", fontSize: "17px", color: "black", background: '#FFFFFF' } }),
    },
  ];

  const data = friends.map((friend) => ({
    key: friend.id,
    username: friend.username,
    id: friend.id,
  }));

  return (
    <div style={{ display: 'flex', alignItems: 'flex-start' }}> 
      <Card style={{ width: '15%', height: '745px', right: '25px', bottom: '62px', backgroundImage: "linear-gradient(to top,  #ffffff, #ffffff)", borderRadius: '10px' }}> </Card>
      <div style={{ width: '90%', marginLeft: '0%', marginTop: '30px' }}> 
        <Table columns={columns} dataSource={data} pagination={false} size="middle" />
      </div>
    </div>
  );
}

export default FriendsListTable;
