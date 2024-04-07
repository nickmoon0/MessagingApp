const buttonsConfig = [
    {
      key: 'home',
      navigateTo: '/home',
      iconSrc: 'https://cdn.lordicon.com/laqlvddb.json',
      tooltip: 'Home',
      borderColor: '#3dc6fc',
    },
    {
      key: 'friends',
      navigateTo: '/friends',
      iconSrc: 'https://cdn.lordicon.com/wzrwaorf.json',
      tooltip: 'Friends',
      borderColor: '#3dc6fc',
    },
    {
      key: 'messages',
      navigateTo: '/friends',
      iconSrc: 'https://cdn.lordicon.com/aycieyht.json',
      tooltip: 'Messages',
      borderColor: '#3dc6fc',
    },
    {
      key: 'activity',
      navigateTo: '/friends',
      iconSrc: 'https://cdn.lordicon.com/vspbqszr.json',
      tooltip: 'Activity',
      borderColor: '#3dc6fc',
    },
    {
      key: 'settings',
      navigateTo: '/friends',
      iconSrc: 'https://cdn.lordicon.com/lecprnjb.json',
      tooltip: 'Settings',
      borderColor: '#2ba2ff',
    },
    {
      key: 'info',
      navigateTo: '/friends',
      iconSrc: 'https://cdn.lordicon.com/yxczfiyc.json',
      tooltip: 'Info',
      borderColor: '#2ba2ff',
    },
    {
      key: 'logout',
      navigateTo: '/friends',
      iconSrc: 'https://cdn.lordicon.com/rmkahxvq.json',
      tooltip: 'Log Out',
      borderColor: '#2ba2ff',
      action: () => {
        localStorage.removeItem('token');
        console.log("token removed boiii");
      }
    }
  ];

  export default buttonsConfig;