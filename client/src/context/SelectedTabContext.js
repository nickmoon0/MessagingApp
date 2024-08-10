// src/context/SelectedTabContext.js
import React, { createContext, useContext, useState } from 'react';

const SelectedTabContext = createContext();

export const useSelectedTab = () => useContext(SelectedTabContext);

export const SelectedTabProvider = ({ children }) => {
  const [selectedTab, setSelectedTab] = useState('Friends');

  return (
    <SelectedTabContext.Provider value={{ selectedTab, setSelectedTab }}>
      {children}
    </SelectedTabContext.Provider>
  );
};
