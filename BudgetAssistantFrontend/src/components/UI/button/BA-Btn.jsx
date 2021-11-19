import React from "react";
import Button from "@mui/material/Button";

const BABtn = (props) => {
  return <Button {...props}>{props.children}</Button>;
};

export default BABtn;
