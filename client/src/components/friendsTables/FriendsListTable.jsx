import React, { useRef, useState } from 'react';
import { Table, Button, Input, Space, Tooltip, Card } from 'antd';
import { SearchOutlined, DeleteOutlined, MessageOutlined} from '@ant-design/icons';
import Highlighter from 'react-highlight-words';
import { MagnifyingGlass } from 'phosphor-react';

const FriendsListTable = ({ friends }) => {
  const [searchText, setSearchText] = useState('');
  const [searchedColumn, setSearchedColumn] = useState('');
  const searchInput = useRef(null);

  const handleSearch = (selectedKeys, confirm, dataIndex) => {
    confirm();
    setSearchText(selectedKeys[0]);
    setSearchedColumn(dataIndex);
  };

  const handleReset = (clearFilters) => {
    clearFilters();
    setSearchText('');
  };

  const getColumnSearchProps = (dataIndex) => ({
    filterDropdown: ({ setSelectedKeys, selectedKeys, confirm, clearFilters }) => (
        <div style={{ padding: 8}}>
        <Input
          ref={searchInput}
          placeholder={`Search ${dataIndex}`}
          value={selectedKeys[0]}
          onChange={(e) => setSelectedKeys(e.target.value ? [e.target.value] : [])}
          onPressEnter={() => handleSearch(selectedKeys, confirm, dataIndex)}
          style={{ marginBottom: 8, display: 'block' }}
        />
        <Space>
          <Button
            onClick={() => handleSearch(selectedKeys, confirm, dataIndex)}
            icon={<SearchOutlined />}
            size="small"
            backgroundColor="black"
            style={{ width: 90, borderRadius:"5px" }}
            
          >
            Search
          </Button>
          <Button onClick={() => handleReset(clearFilters)} size="small" style={{ width: 90, borderRadius:"5px" }}>
            Reset
          </Button>
        </Space>
      </div>
    ),
    filterIcon: () => (
       <div className="no-hover-effect">
            <MagnifyingGlass className="no-hover-effect" style={{ color: '#18181B', fontSize: '21px', textAlign: 'right' }} />
            </div>
    ),
    onFilter: (value, record) =>
      record[dataIndex] ? record[dataIndex].toString().toLowerCase().includes(value.toLowerCase()) : '',
    onFilterDropdownOpenChange: (visible) => {
      if (visible) {
        setTimeout(() => searchInput.current.select(), 100);
      }
    },
    render: (text) =>
      searchedColumn === dataIndex ? (
        <Highlighter
          highlightStyle={{ backgroundColor: '#ffc069', padding: 0 }}
          searchWords={[searchText]}
          autoEscape
          textToHighlight={text ? text.toString() : ''}
        />
      ) : (
        text
      ),
  });

  const columns = [
    {
      title: `Friends - ${friends.length}`,
      dataIndex: 'username',
      key: 'username',
      ...getColumnSearchProps('username'),
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
      onHeaderCell: () => ({ style: { bottom: "10px", fontSize: "17px", color: "black", background: '#FFFFFF' } }),
    },
  ];

  const data = friends.map((friend) => ({
    key: friend.id,
    username: friend.username,
    id: friend.id,
  }));

  return (
    <div style={{ display: 'flex', alignItems: 'flex-start' }}> 
      <Card style={{ width: '15%', height: '745px', right: '25px', bottom: '25px', backgroundImage: "linear-gradient(to top,  #ffffff, #ffffff)", borderRadius: '10px' }}> </Card>
      <div style={{ width: '90%', marginLeft: '0%', marginTop: '60px' }}> 
        <Table columns={columns} dataSource={data} pagination={false} size="middle" />
      </div>
    </div>
  );
}

export default FriendsListTable;
