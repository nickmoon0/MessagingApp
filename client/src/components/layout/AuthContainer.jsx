import React from 'react';

const AuthContainer = ({ children }) => {
    return (
      <div className="body"> {/* This div will have the background image */}
        <div className="container">
          <div className="login-card"> 
            <div className="login-container">
              <div class="login-content">
                <div className="chat-bubble">
                  {children}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  };
  
  export default AuthContainer;