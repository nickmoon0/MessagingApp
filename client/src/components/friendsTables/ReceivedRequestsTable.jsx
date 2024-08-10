import React from 'react';
import { Table, Button, Card } from 'antd';
import { CheckOutlined, CloseOutlined  } from '@ant-design/icons';


const ReceivedRequestsTable = ({ requests, onAccept }) => {
    const columns = [
      {
        title: `Friend Requests - ${requests.length}`,
        dataIndex: 'username',
        key: 'username',
        render: (text) => <span style={{ fontSize: "14.5px", fontWeight:"600"}}>{text}</span>,
        onHeaderCell: () => ({ style: { bottom:"-3px", fontSize: "18px", color: "black", background: '#FFFFFF', fontWeight:"600",  border: "black" } }),
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
        onHeaderCell: () => ({ style: { bottom:"-3px", textAlign: 'right', fontSize: "18px", color: "black", background: '#FFFFFF',  border: "black" } }),
      },
    ];

    const data = requests.map(request => ({
      key: request.friendRequestId, 
      username: request.username,
      id: request.friendRequestId,
    }));
  
    return (
      <div style={{ display: 'flex', alignItems: 'flex-start' }}> 
      <Card style={{ width: '15%', height: '745px', right: '25px', bottom: '62px', backgroundImage: "linear-gradient(to top,  #ffffff, #ffffff)", borderRadius: '10px' }}> </Card>
      <div style={{ width: '90%', marginLeft: '0%', marginTop: '15px' }}> 
        <Table 
          size="middle"
          columns={columns} 
          dataSource={data} 
          pagination={false}
        />
      </div>
      </div>
    );
  };
  
  export default ReceivedRequestsTable;