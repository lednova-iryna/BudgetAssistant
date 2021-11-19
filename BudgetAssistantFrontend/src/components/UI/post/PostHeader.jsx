import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableCell, { tableCellClasses } from "@mui/material/TableCell";
import React from 'react';
import { useTableStyles } from "../../../hooks/useTableStyle";

const PostHeader = () => {
    const classes = useTableStyles();
    return (
        <TableHead>
        <TableRow>
          <TableCell className={classes.tableHeaderCell}>Date</TableCell>
          <TableCell className={classes.tableHeaderCell}>Category</TableCell>
          <TableCell className={classes.tableHeaderCell}>Description</TableCell>
          <TableCell className={classes.tableHeaderCell}>Amount</TableCell>
          <TableCell className={classes.tableHeaderCell}>Type</TableCell>
          <TableCell className={classes.tableHeaderCell}>Actions</TableCell>
        </TableRow>
      </TableHead>
    );
}

export default PostHeader;
