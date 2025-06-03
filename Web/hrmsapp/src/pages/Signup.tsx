import React, { useState } from 'react';

const Signup = () => {
  const [formData, setFormData] = useState({
    lastName: '',
    email: '',
    countryCode: '+86',
    phoneNumber: '',
    password: '',
    confirmPassword: '',
    termsAccepted: false
  });
  
  const [errors, setErrors] = useState({
    lastName: '',
    email: '',
    phoneNumber: '',
    password: '',
    confirmPassword: '',
    termsAccepted: ''
  });
  
  const [touchedFields, setTouchedFields] = useState({});
  
  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    const fieldValue = type === 'checkbox' ? checked : value;
    
    setFormData({
      ...formData,
      [name]: fieldValue
    });
    
    // Clear error when typing
    if (errors[name]) {
      setErrors({
        ...errors,
        [name]: ''
      });
    }
  };
  
  const handleBlur = (e) => {
    const { name, value } = e.target;
    setTouchedFields({
      ...touchedFields,
      [name]: true
    });
    
    // Validate on blur
    validateField(name, value);
  };
  
  const validateField = (name: string, value: string) => {
    let errorMessage = '';
    
    switch (name) {
      case 'lastName':
        errorMessage = !value.trim() ? 'Last name is required' : '';
        break;
      case 'email':
        errorMessage = !value.trim() ? 'Email is required' : '';
        break;
      case 'phoneNumber':
        errorMessage = !value.trim() ? 'Phone number is required' : '';
        break;
      case 'password':
        errorMessage = !value ? 'Password is required' : '';
        break;
      case 'confirmPassword':
        errorMessage = !value ? 'Please confirm your password' : '';
        break;
      default:
        break;
    }
    
    setErrors({
      ...errors,
      [name]: errorMessage
    });
    
    return errorMessage === '';
  };
  
  const handleSubmit = () => {
    // Mark all fields as touched
    const allFields = {
      lastName: true,
      email: true,
      phoneNumber: true,
      password: true,
      confirmPassword: true,
      termsAccepted: true
    };
    
    setTouchedFields(allFields);
    
    // Validate all fields
    let formIsValid = true;
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const newErrors = {...errors};
    
    Object.keys(formData).forEach(field => {
      if (!validateField(field, formData[field])) {
        formIsValid = false;
      }
    });
    
    if (formIsValid) {
      console.log('Form submitted:', formData);
      // Submit form
    }
  };
  
  // Styles to match screenshot exactly
  const inputStyle = {
    width: '100%',
    padding: '10px',
    border: '1px solid #ffcdd2',
    borderRadius: '6px',
    fontSize: '14px',
    marginBottom: '2px'
  };
  
  const labelStyle = {
    fontSize: '14px',
    display: 'block',
    marginBottom: '6px'
  };
  
  const errorStyle = {
    color: '#e53935',
    fontSize: '13px',
    marginTop: '2px',
    marginBottom: '12px'
  };
  
  return (
    <div className="bg-white p-6 max-w-md mx-auto">
      <div>
        {/* Last Name */}
        <div>
          <label htmlFor="lastName" style={labelStyle}>Last name</label>
          <input
            type="text"
            id="lastName"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            onBlur={handleBlur}
            style={inputStyle}
          />
          {errors.lastName && <div style={errorStyle}>Last name is required</div>}
        </div>
        
        {/* Email */}
        <div>
          <label htmlFor="email" style={labelStyle}>Email address </label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            onBlur={handleBlur}
            style={inputStyle}
          />
          {errors.email && <div style={errorStyle}>Email is required</div>}
        </div>
        
        {/* Phone Number */}
        <div>
          <label htmlFor="phoneNumber" style={labelStyle}>Phone Number *</label>
          <div style={{display: 'flex'}}>
            <select 
              value={formData.countryCode}
              onChange={(e) => setFormData({...formData, countryCode: e.target.value})}
              style={{
                padding: '10px', 
                border: '1px solid #ffcdd2', 
                borderRight: 'none',
                borderRadius: '6px 0 0 6px',
                background: '#f5f5f5'
              }}
            >
              <option value="+86">+86</option>
              <option value="+91">+91</option>
              <option value="+1">+1</option>
            </select>
            <input
              type="tel"
              id="phoneNumber"
              name="phoneNumber"
              placeholder="10-digit mobile number"
              value={formData.phoneNumber}
              onChange={handleChange}
              onBlur={handleBlur}
              style={{
                flex: 1,
                padding: '10px',
                border: '1px solid #ffcdd2',
                borderRadius: '0 6px 6px 0',
                fontSize: '14px'
              }}
            />
          </div>
          {errors.phoneNumber && <div style={errorStyle}>Phone number is required</div>}
        </div>
        
        {/* Password */}
        <div>
          <label htmlFor="password" style={labelStyle}>Password</label>
          <input
            type="password"
            id="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            onBlur={handleBlur}
            style={inputStyle}
          />
          {errors.password && <div style={errorStyle}>Password is required</div>}
        </div>
        
        {/* Confirm Password */}
        <div>
          <label htmlFor="confirmPassword" style={labelStyle}>Confirm password</label>
          <input
            type="password"
            id="confirmPassword"
            name="confirmPassword"
            value={formData.confirmPassword}
            onChange={handleChange}
            onBlur={handleBlur}
            style={inputStyle}
          />
          {errors.confirmPassword && <div style={errorStyle}>Please confirm your password</div>}
        </div>
        
        {/* Terms and Conditions */}
        <div style={{marginTop: '12px', marginBottom: '20px'}}>
          <label style={{display: 'flex', alignItems: 'center'}}>
            <input
              type="checkbox"
              name="termsAccepted"
              checked={formData.termsAccepted}
              onChange={handleChange}
              style={{marginRight: '8px'}}
            />
            <span style={{fontSize: '14px'}}>
              I agree to the <a href="#" style={{color: '#5c6bc0', textDecoration: 'none'}}>Terms and Conditions</a>
            </span>
          </label>
        </div>
        
        {/* Register Button */}
        <button 
          onClick={handleSubmit}
          style={{
            width: '100%',
            padding: '10px',
            backgroundColor: '#5c6bc0',
            color: 'white',
            border: 'none',
            borderRadius: '6px',
            fontSize: '15px',
            fontWeight: '500',
            cursor: 'pointer'
          }}
        >
          Register
        </button>
      </div>
    </div>
  );
};

export default Signup;