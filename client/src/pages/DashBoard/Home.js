import React from 'react';
import { Card, Empty } from 'antd';

const Home = () => {
  return (
    <div 
      style={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        height: '50vh' }}>
      <Card 
        style={{ 
          width: '100px',
          height: '100px', 
          display: 'flex', 
          justifyContent: 'center', 
          alignItems: 'center' }}>
        <Empty />
      </Card>
    </div>
  );
};

export default Home;
