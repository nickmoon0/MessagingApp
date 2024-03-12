// hooks/useFormState.js
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
    try {
      await onSubmit(formData); // Adjust based on how you're handling API calls
    } catch (error) {
      setError(error.message);
    }
  };

  return { formData, handleChange, handleSubmit, error };
};
