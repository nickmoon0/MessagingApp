import React, { useState } from 'react';
import UserSearch from './UserSearch';
import { Segmented, Card, Empty} from 'antd';
import SentRequests from './SentRequests';
import { acceptFriendRequest } from '../../api/userService';
import useReceivedRequests from '../../hooks/useReceivedRequests';
import ReceivedRequestsTable from '../../components/ReceivedRequestsTable';
import { UserOutlined, CheckOutlined, LoadingOutlined } from '@ant-design/icons';
import '../../styles/center.css'; 
import './AddFriend.css';


function Friends() {
    const { receivedRequests, handleRequestSent } = useReceivedRequests();
    const [selectedTab, setSelectedTab] = useState('All');

    const onSegmentChange = (key) => 
    {
        setSelectedTab(key);
    };

    return (
        <div>
            <UserSearch onRequestSent={handleRequestSent} />
            <div style={{ marginLeft: "250px", marginTop: "60px", position: 'relative' }}>
            <div className="w-full flex-col">
                <Segmented 
                className="custom-segmented"
                    size="default"
                    options={[{
                      label: 'Friends', value: 'Friends', icon:<UserOutlined />}, 
                      {label: 'Received', value: 'Received', icon:<CheckOutlined />},
                      {label:'Sent',value: 'Sent', icon:<LoadingOutlined />}]}
                    value={selectedTab}
                    onChange={onSegmentChange}
                    style={{ position: 'absolute', left: 31, top: -32, zIndex: 1000, background:"#E6F1FE", borderRadius: '10px' }} // Add some space below the segmented control
                    
                />

                {selectedTab === 'Friends' && (
                    <Card className="fixed-size-card" style={{ border: 'none', boxShadow: 'none' }}>
                        <Empty style={{marginTop: '150px'}}/>
                    </Card>
                )}
                {selectedTab === 'Received' && (
                    <Card  className="fixed-size-card" style={{ border: 'none', boxShadow: 'none' }}>
                        <ReceivedRequestsTable requests={receivedRequests} onAccept={acceptFriendRequest} />
                    </Card>
                )}
                {selectedTab === 'Sent' && (
                    <Card className="fixed-size-card" style={{ border: 'none', boxShadow: 'none' }}>
                        <SentRequests />
                    </Card>
                )}

            </div>
            </div>
        </div>
    );
}

export default Friends;
