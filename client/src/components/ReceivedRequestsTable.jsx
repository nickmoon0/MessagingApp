import React from 'react';
import { Table, Button } from 'antd';
import { CheckOutlined, CloseOutlined  } from '@ant-design/icons';


const ReceivedRequestsTable = ({ requests, onAccept }) => {
    // Define the columns for the Ant Design table
    const columns = [
      {
        title: `Friend Requests - ${requests.length}`,
        dataIndex: 'username',
        key: 'username',
        render: (text) => <span style={{ fontSize: "14px" }}>{text}</span>,
        onHeaderCell: () => ({ style: { bottom:"10px", fontSize: "16px", color: "black", background: '#FFFFFF' } }),
      },
      {
        title: '',
        key: 'action',
        align: 'right', 
        render: (text, record) => (
            <>
            <Button 
              type="danger"
              shape="circle"
              icon={<CloseOutlined />}
              style={{ marginRight: 8}}
              className="reject-button" 
            />
            <Button 
            
              auto
              shape="circle"
              onClick={() =>onAccept(record.id)}
              icon={<CheckOutlined />}
              className="accept-button"
            />
          </>
            
        ),
        onHeaderCell: () => ({ style: { bottom:"10px", textAlign: 'right', fontSize: "16px", color: "#18181B", background: '#FFFFFF' } }),
      align: 'right',
      },
    ];

    const data = requests.map(request => ({
      key: request.friendRequestId, 
      username: request.username,
      id: request.friendRequestId,
    }));
  
    return (
      <Table 
        size="small"
        columns={columns} 
        dataSource={data} 
        pagination={false}
      />
    );
  };
  
  export default ReceivedRequestsTable;