import React, { useEffect, useState } from 'react';

interface CheckboxFilterProps {
  api: any; // Adjust 'any' to the specific type if known
  filterChangedCallback: () => void;
}

interface FilterModel {
  filter: string | null;
}

const CheckboxFilter: React.FC<CheckboxFilterProps> = (props) => {
  const [filter, setFilter] = useState<string | null>(null);

  useEffect(() => {
    props.filterChangedCallback();
  }, [filter]);

  const onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newFilter = event.target.value;
    setFilter(newFilter);

    // If "Yes" is selected, get all checked rows
    if (newFilter === 'yes') {
      getCheckedRows();
    }
  };

  const getCheckedRows = () => {
    const selectedNodes = props.api.getSelectedNodes();
    const checkedRows = selectedNodes.map((node: any) => node.data); // Adjust 'any' to the specific type if known
    console.log('Checked rows:', checkedRows);
  };

  const doesFilterPass = (params: any) => { // Adjust 'any' to the specific type if known
    if (filter === null) return true;
    const isChecked = props.api.getSelectedNodes().some((node: any) => node.data === params.data); // Adjust 'any' to the specific type if known
    return filter === 'yes' ? isChecked : !isChecked;
  };

  const isFilterActive = () => {
    return filter !== null;
  };

  const getModel = (): FilterModel => {
    return { filter };
  };

  const setModel = (model: FilterModel | null) => {
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
