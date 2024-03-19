import { useState } from 'react';

export const useFormState = (initialFormData, onSubmit) => {
  const [formData, setFormData] = useState(initialFormData);
  const [error, setError] = useState('');

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    try 
    {
      await onSubmit(formData); 
    } 
    catch (error) 
    {
      setError(error.message);
    }
  };

  return { formData, handleChange, handleSubmit, error};
};
