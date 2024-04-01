import React from 'react';
import { Table, Button, Card } from 'antd';
import { CheckOutlined, CloseOutlined  } from '@ant-design/icons';


const ReceivedRequestsTable = ({ requests, onAccept }) => {
    const columns = [
      {
        title: `Friend Requests - ${requests.length}`,
        dataIndex: 'username',
        key: 'username',
        render: (text) => <span style={{ fontSize: "14px", fontWeight:"600"}}>{text}</span>,
        onHeaderCell: () => ({ style: { bottom:"10px", fontSize: "16px", color: "black", background: '#FFFFFF', fontWeight:"600" } }),
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
      },
    ];

    const data = requests.map(request => ({
      key: request.friendRequestId, 
      username: request.username,
      id: request.friendRequestId,
    }));
  
    return (
      <div style={{ display: 'flex', alignItems: 'flex-start' }}> 
      <Card style={{ width: '15%', height: '745px', right: '25px', bottom: '25px', backgroundImage: "linear-gradient(to top,  #ffffff, #ffffff)", borderRadius: '10px' }}> </Card>
      <div style={{ width: '90%', marginLeft: '0%', marginTop: '60px' }}> 
        <Table 
          size="small"
          columns={columns} 
          dataSource={data} 
          pagination={false}
        />
      </div>
      </div>
    );
  };
  
  export default ReceivedRequestsTable;