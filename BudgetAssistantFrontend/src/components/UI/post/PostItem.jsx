import { TableCell } from "@mui/material";
import { TableRow } from "material-ui";
import React from "react";
import { styled } from "@mui/material/styles";
import { useTableStyles } from "../../../hooks/useTableStyle";
import PostButtons from "./PostButtons";

const PostItem = ({ post }) => {
  // const StyledTableRow = styled(TableRow)(({ theme }) => ({
  //   "&:nth-of-type(odd)": {
  //     backgroundColor: theme.palette.action.hover,
  //   },
  // }));

  const classes = useTableStyles();
  return (
    <TableRow className={classes.tableRow}>
      <TableCell>{post.date.format("DD/MM/YYYY")}</TableCell>
      <TableCell>{post.category}</TableCell>
      <TableCell>{post.description}</TableCell>
      <TableCell>{post.amount}</TableCell>
      <TableCell>{post.postType}</TableCell>
      <TableCell className={classes.tableButtonCell}>
        <PostButtons></PostButtons>
      </TableCell>
    </TableRow>
  );
};

export default PostItem;
