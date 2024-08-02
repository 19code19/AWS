import React, { useEffect, useState } from 'react';

const CheckboxFilter = (props) => {
  const [filter, setFilter] = useState(null);

  useEffect(() => {
    props.filterChangedCallback();
  }, [filter]);

  const onChange = (event) => {
    setFilter(event.target.value);
  };

  const doesFilterPass = (params) => {
    if (filter === null) return true;
    const isChecked = props.api.getSelectedNodes().some(node => node.data === params.data);
    return filter === 'yes' ? isChecked : !isChecked;
  };

  const isFilterActive = () => {
    return filter !== null;
  };

  const getModel = () => {
    return { filter };
  };

  const setModel = (model) => {
    setFilter(model ? model.filter : null);
  };

  return (
    <div>
      <label>
        <input
          type="radio"
          name="filter"
          value="yes"
          checked={filter === 'yes'}
          onChange={onChange}
        />
        Yes
      </label>
      <label>
        <input
          type="radio"
          name="filter"
          value="no"
          checked={filter === 'no'}
          onChange={onChange}
        />
        No
      </label>
    </div>
  );
};

export default CheckboxFilter;
