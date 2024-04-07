import React, {useState} from 'react';
import { Card, Segmented, Input, Button } from 'antd';
import { FiSearch } from "react-icons/fi";
import { useFriends } from '../../hooks/useFriends';
import FriendsListTable from '../../components/friendsTables/FriendsListTable';
import ReceivedRequestsTable from '../../components/friendsTables/ReceivedRequestsTable';
import SentRequests from './SentRequests';
import UserSearch from './UserSearch';
import useReceivedRequests from '../../hooks/useReceivedRequests';


function Friends() {
  const { selectedTab, setSelectedTab, searchText, setSearchText, friends, handleAcceptFriendRequest } = useFriends();
  const [showSearch, setShowSearch] = useState(false);
  const { receivedRequests, handleRequestSent, setReceivedRequests } = useReceivedRequests();


  const handleSearchIconClick = () => setShowSearch(!showSearch);
  const onSearchChange = (e) => setSearchText(e.target.value.toLowerCase());

  const filteredFriends = friends.filter(friend => friend.username.toLowerCase().includes(searchText));
  const filteredReceivedRequests = receivedRequests.filter(request => request.username.toLowerCase().includes(searchText));

return (
  <div>
    <UserSearch onRequestSent={handleRequestSent} />
    <div style={{ marginLeft: "150px", position: 'relative' }}>
      <Segmented
        className="custom-segmented"
        size="default"
        options={[{ label: 'Friends', value: 'Friends' }, { label: 'Received', value: 'Received' }, { label: 'Sent', value: 'Sent' }]}
        value={selectedTab}
        onChange={setSelectedTab}
        style={{ position: 'absolute', backgroundColor: '#ffffff', left: 168, top: 70, zIndex: 1000, fontWeight: 500, fontSize: '17.5px', borderRadius: '5px' }}
      />

      {selectedTab === 'Friends' && (
        <Card className="fixed-size-card" style={{ border: 'none', boxShadow: 'none' }}>
          <div style={{ display: 'flex',
                        justifyContent: 'flex-end',
                        paddingRight: '120px',
                        paddingTop:'3.5px',
                        borderRadius:'10px',
                        }}>
            {showSearch ? (
                    <>
                        <Input.Search
                            placeholder="Search friends"
                            onChange={onSearchChange}
                            onBlur={() => setShowSearch(false)}
                            autoFocus 
                            allowClear
                            style={{ width: 300 }}
                        />
                    </>
                    ) : (
                        <Button
                        onClick={handleSearchIconClick}
                        style={{ backgroundColor: '#2898fb', border: 'none' }} 
                        icon={<FiSearch style={{ color: 'white', fontSize: '20px', marginTop:'1px' }} />} 
                      />
              )}         
          </div>
          <FriendsListTable friends={filteredFriends} />
        </Card>
      )}
      {selectedTab === 'Received' && (
            <Card className="fixed-size-card" style={{ border: 'none', boxShadow: 'none' }}>
            <div style={{ display: 'flex',
                          justifyContent: 'flex-end',
                          paddingRight: '120px',
                          paddingTop:'3.5px',
                          borderRadius:'10px',
                          }}>
            {showSearch ? (
                    <>
                        <Input.Search
                            placeholder="Search Requests..."
                            onChange={onSearchChange}
                            onBlur={() => setShowSearch(false)}
                            autoFocus 
                            allowClear
                            style={{ width: 300 }}
                        />
                    </>
                    ) : (
                        <Button
                        onClick={handleSearchIconClick}
                        style={{ backgroundColor: '#2898fb', border: 'none' }} // Set the button's background and border color to blue
                        icon={<FiSearch style={{ color: 'white', fontSize: '20px', marginTop:'1px' }} />} // Set the icon's color to white and size to 24px
                      />
                      )}      
          </div>
          <ReceivedRequestsTable requests={filteredReceivedRequests} onAccept={handleAcceptFriendRequest} />
        </Card>
      )}
      {selectedTab === 'Sent' && (
        <Card className="fixed-size-card" style={{ border: 'none', boxShadow: 'none' }}>
            <SentRequests searchText={searchText} onSearchChange={onSearchChange}/>
        </Card>
      )}
    </div>
  </div>
  );
}

export default Friends;
