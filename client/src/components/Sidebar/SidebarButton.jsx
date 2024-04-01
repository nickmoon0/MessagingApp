import React, { useState, useEffect } from 'react';
import { Button, Tooltip } from 'antd';
import { useNavigate } from 'react-router-dom';

const SidebarButton = ({ navigateTo, iconSrc, tooltip, borderColor, isSelected, onClick, action }) => {
    const navigate = useNavigate();
    const [visible, setVisible] = useState(false);
    const [hovering, setHovering] = useState(false);
  
    const handleClick = () => {
      if (action) {
        action();
        navigate('/'); 
      } else {
        navigate(navigateTo); 
      }
      onClick(); 
    };
  
    useEffect(() => {
      let timer;
      if (hovering) {
        timer = setTimeout(() => {
          setVisible(true);
        }, 2500); //display after 2.5 secs
      } else {
        setVisible(false); //hide tooltip when not hovering
      }
  
      return () => clearTimeout(timer); 
    }, [hovering]);
  
    return (
      <Tooltip placement="right" title={tooltip} visible={visible}>
        <Button
          overlayClassName="tooltip-no-arrow"
         onMouseEnter={() => setHovering(true)} 
         onMouseLeave={() => setHovering(false)} 
         onClick={() => { handleClick(); onClick();  }}
          shape="square"
          style={{
            marginBottom: '10px',
            background: 'white',
            color: '#27272A',
            padding: '2px',
            display: 'inline-flex',
            justifyContent: 'center',
            alignItems: 'center',
            width: '43px',
            height: '40px',
            boxShadow: isSelected ? `0 0 0 3px #FFF` : 'none',
            border: isSelected ? `2.5px solid ${borderColor}` : `2px solid transparent`,
          }}
        >
          <lord-icon
            src={iconSrc}
            stroke = "bold"
            trigger="click"
            colors="primary:#000000,secondary:#1663c7"
            style={{ width: '33px', height: '33px' }}
          ></lord-icon>
        </Button>
      </Tooltip>
    );
  };

  export default SidebarButton;