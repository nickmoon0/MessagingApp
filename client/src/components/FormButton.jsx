import React from 'react';
import { Button } from "@nextui-org/react";

const FormButton = ({ text, onClick, type, style, isDisabled }) => (
  <Button
    type={type || "button"}
    onClick={onClick}
    style={{
      width: '100%',
      backgroundColor: '#338EF7',
      color: '#FAFAFA',
      fontWeight: 'bold',
      ...style,
    }}
        disabled={isDisabled}
  >
    {text}
  </Button>
);

export default FormButton;
