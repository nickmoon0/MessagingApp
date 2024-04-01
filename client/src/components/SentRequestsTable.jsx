import React from 'react';
import { Table, Button, Tooltip } from 'antd';
import { CloseOutlined } from '@ant-design/icons';


const SentRequestsTable = ({ data, onCancelRequest }) => {
  const columns = [
    {
      title: `Pending Requests - ${data.length}`, // Dynamically set the title
      dataIndex: 'username',
      key: 'username',
      render: (text) => <span style={{ fontSize: "14px" }}>{text}</span>,
      onHeaderCell: () => ({ style: { bottom:"10px", fontSize: "16px", color: "black", background: '#FFFFFF' } }),
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
      onHeaderCell: () => ({ style: { bottom:"10px", textAlign: 'right', fontSize: "16px", color: "#18181B", background: '#FFFFFF' } }),
      align: 'right',
    },
  ];

  return (
    <Table
      size="small"
      columns={columns}
      dataSource={data.map(item => ({...item, key: item.id}))}
      pagination={false}
      aria-label="Sent Friend Requests"
    />
  );
};

export default SentRequestsTable;
