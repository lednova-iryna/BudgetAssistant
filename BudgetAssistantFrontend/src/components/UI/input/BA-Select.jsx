import React, { useState } from "react";
import MenuItem from "@mui/material/MenuItem";
import { FormControl, InputLabel, Select } from "@mui/material";

const BASelect = ({ options, label, onChange }) => {
  return (
    <FormControl variant='outlined'  sx={{ m: 0, minWidth: 120 }}>
      <InputLabel>{label}</InputLabel>
      <Select onChange={onChange}>
        <MenuItem value="">
          <em>None</em>
        </MenuItem>
        {options.map((option) => (
          <MenuItem value={option.value}>{option.name}</MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};

export default BASelect;
