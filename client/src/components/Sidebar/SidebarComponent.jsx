import React, { useState } from 'react';
import { Layout} from 'antd';
import { defineElement } from '@lordicon/element';
import Lottie from 'lottie-web';
import SidebarButton from './SidebarButton';
import buttonsConfig from './buttonsConfig';


defineElement(Lottie.loadAnimation);
const { Sider } = Layout;


const SidebarComponent = () => {
  const [selectedKey, setSelectedKey] = useState(null);
  return (
    <Sider
      width={70}
      style={{
        overflow: 'auto',
        height: 'calc(100vh - 40px)',
        position: 'fixed',
        left: 0,
        top: 40,
        backgroundImage: 'linear-gradient(to bottom, #33ccff, #0099ff)',
      }}
    >
      <div style={{ marginTop: '10px' }}>
        <div
          style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            height: '100%',
          }}
        >
          {buttonsConfig.map(({ key, navigateTo, iconSrc, tooltip, borderColor, action }) => (
            <SidebarButton
              key={key}
              navigateTo={navigateTo}
              iconSrc={iconSrc}
              tooltip={tooltip}
              borderColor={borderColor}
              isSelected={selectedKey === key}
              onClick={() => setSelectedKey(key)}
              action={action}
            />
          ))}
        </div>
      </div>
    </Sider>
  );
};

export default SidebarComponent;
