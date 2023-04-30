import { ToggleButton, ToggleButtonGroup } from "@mui/material";
import React from "react";

const BAToggleBtn = ({ keyProp, values, currentValue, onChange }) => {
  const children = values.map((element) => {
    return (
      <ToggleButton value={element} key={keyProp + element}>
        {element}
      </ToggleButton>
    );
  });

  const control = {
    value: currentValue,
    onChange,
    exclusive: true,
  };

  return (
    <ToggleButtonGroup size="large" {...control}>
      {children}
    </ToggleButtonGroup>
  );
};

export default BAToggleBtn;
