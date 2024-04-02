import React from 'react';
import { Table, Button, Tooltip, Card } from 'antd';
import { CloseOutlined } from '@ant-design/icons';

const SentRequestsTable = ({ data, onCancelRequest }) => {
  const columns = [
    {
      title: `Pending Requests - ${data.length}`, 
      dataIndex: 'username',
      key: 'username',
      render: (text) => <span style={{ fontSize: "15px", fontWeight:"600" }}>{text}</span>,
      onHeaderCell: () => ({ style: { bottom:"8px", fontSize: "17px", color: "black", background: '#FFFFFF' } }),
    },
    {
      title: '',
      dataIndex: 'action',
      key: 'action',
      render: (_, record) => (
        <>
          <Tooltip placement="left" title="Cancel Request">
            <Button
              type="danger"
              shape="circle"
              icon={<CloseOutlined />}
              onClick={() => onCancelRequest(record.key)}
              className="reject-button"
              style={{ marginRight: "7px" }}
            />
          </Tooltip>
          <Button loading={true} style={{ marginLeft: 'auto' }}>
            Request Sent
          </Button>
        </>
      ),
      onHeaderCell: () => ({ style: { bottom:"8px", textAlign: 'right', fontSize: "17px", color: "black", background: '#FFFFFF' } }),
      align: 'right',
    },
  ];

  return (
    <div style={{ display: 'flex', alignItems: 'flex-start' }}> 
    <Card style={{ width: '15%', height: '745px', right: '25px', bottom: '62px', backgroundImage: "linear-gradient(to top,  #ffffff, #ffffff)", borderRadius: '10px' }}> </Card>
    <div style={{ width: '90%', marginLeft: '0%', marginTop: '30px' }}> 
    <Table
      size="middle"
      columns={columns}
      dataSource={data.map(item => ({...item, key: item.id}))}
      pagination={false}
      aria-label="Sent Friend Requests"
    />
    </div>
    </div>
  );
};

export default SentRequestsTable;
