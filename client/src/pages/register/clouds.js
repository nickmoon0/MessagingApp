import React from 'react';
import { motion } from 'framer-motion';

const AnimatedCloud = ({ src, className}) => {
    return (
        <motion.img 
          src={src} 
          alt="Cloud" 
          className={className}
          whileHover={{ scale: 1.1 }} 
          initial={{ y: -200 }} 
          animate={{ y: -200 }} 
          transition={{ type: "spring", stiffness: 300 }}
        />
      );
    };

export default AnimatedCloud;