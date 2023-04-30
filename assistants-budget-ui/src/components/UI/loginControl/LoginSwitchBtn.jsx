import React from "react";
import Switch from "@mui/material/Switch";
import FormControlLabel from "@mui/material/FormControlLabel";
import FormGroup from "@mui/material/FormGroup";

const LoginSwitchBtn = ({ checked, onChange }) => {
  return (
    <FormGroup>
      <FormControlLabel
        control={
          <Switch
            checked={checked}
            onChange={onChange}
            aria-label="login switch"
          />
        }
        label={checked ? "Logout" : "Login"}
      />
    </FormGroup>
  );
};

export default LoginSwitchBtn;
