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
              className="reject-button" // Apply the CSS class
            />
            <Button 
            
              auto
              shape="circle"
              onClick={() => onAccept(record.id)}
              icon={<CheckOutlined />}
              className="accept-button" // Apply the CSS class
            />
          </>
            
        ),
        onHeaderCell: () => ({ style: { bottom:"10px", textAlign: 'right', fontSize: "16px", color: "#18181B", background: '#FFFFFF' } }),
      align: 'right',
      },
    ];
  
    // Prepare the data for the table
    // Note: Ant Design's Table component automatically assigns a 'key' property to each data row,
    // but if your data does not have a 'key' property, you need to add it.
    // Since you're already using 'id' as a key in your original map, this step might be redundant
    // if your request objects already include an 'id' that can serve as a unique key.
    const data = requests.map(request => ({
      key: request.id, // This is necessary for Ant Design's Table
      username: request.username,
      id: request.id, // Include other data you might want to use in the 'render' function
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